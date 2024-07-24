using ImageProcessingService.Interfaces;
using ImageProcessingService.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks.Dataflow;
using Tensorflow;
using static Tensorflow.Binding;


namespace ImageProcessingService.Services;

public class FaceDetectionService : IFaceDetectionService
{
    private Session session;
    private readonly ILogger<FaceDetectionService> logger;
    private readonly IFileService fileService;

    private BufferBlock<string> dataBLock;
    private TransformBlock<string, ResultResponseWithData<ModelOutput>> processBlock;
    private ActionBlock<ResultResponseWithData<ModelOutput>> sucessActionBlock;
    private ActionBlock<ResultResponseWithData<ModelOutput>> failedActionBlock;

    public FaceDetectionService(
        IFileService fileService,
        ILogger<FaceDetectionService> logger)
    {
        this.fileService = fileService;
        this.logger = logger;
    }

    public Task InitializeAsync()
    {
        string modelPath = this.fileService.PathCombine(Environment.CurrentDirectory, "save_model");

        // Load the model
        var graph = new Graph().as_default();
        this.session = tf.Session(graph);

        if (this.fileService.FileExist(modelPath))
        {
            this.logger.LogError("Not found model for pre-trained data.");
            throw new Exception();
        }

        tf.train.import_meta_graph($"{modelPath}/saved_model.pb");

        this.dataBLock = new BufferBlock<string>();
        this.processBlock = new TransformBlock<string, ResultResponseWithData<ModelOutput>>(async path => await Execute(path));
        this.sucessActionBlock = new ActionBlock<ResultResponseWithData<ModelOutput>>(_ => { });
        this.failedActionBlock = new ActionBlock<ResultResponseWithData<ModelOutput>>(_ => { });

        this.dataBLock.LinkTo(this.processBlock);
        this.processBlock.LinkTo(this.sucessActionBlock, predicate: result => result.IsSuccess);
        this.processBlock.LinkTo(this.failedActionBlock, predicate: result => !result.IsSuccess);

        return Task.CompletedTask;
    }
    public void OutputPrediction(ModelOutput prediction)
    {
        string imageName = Path.GetFileName(prediction.ImagePath);
        this.logger.LogInformation($"Image: {imageName} | Actual Value: {prediction.Label} " +
                                   $"| Predicted Value: {prediction.PredictedLabel}");
    }

    public async Task ProcessImage(string imageDirectory)
    {
        var files = Directory.GetFiles(imageDirectory);
        foreach (var file in files)
        {
            this.dataBLock.Post(file);
        }
        
        this.dataBLock.Complete();
        await this.dataBLock.Completion;
        this.processBlock.Complete();
        await this.processBlock.Completion;
        this.sucessActionBlock.Complete();
        await this.sucessActionBlock.Completion;
        this.failedActionBlock.Complete();
        await this.failedActionBlock.Completion;
    }

    private Task<ResultResponseWithData<ModelOutput>> Execute(string filePath)
    {
        // Load the image
        var tensor = LoadImage(filePath);

        // Detect faces
        var result = DetectFaces(this.session, tensor);

        // Process the result
        bool isRealPerson = result.Any();

        this.logger.LogInformation($"Is real person: {isRealPerson}");
        return null;
    }
    
    private static Tensor LoadImage(string imagePath)
    {
        var fileBytes = File.ReadAllBytes(imagePath);
        var tensor = tf.convert_to_tensor(fileBytes);
        return tensor;
    }
    
    private static IEnumerable<Tensor> DetectFaces(Session session, Tensor tensor)
    {
        var inputTensor = session.graph.OperationByName("image_tensor");
        var outputTensors = new[]
        {
            session.graph.OperationByName("detection_boxes"),
            session.graph.OperationByName("detection_scores"),
            session.graph.OperationByName("detection_classes"),
            session.graph.OperationByName("num_detections")
        };

        var results = session.run(outputTensors, new FeedItem(inputTensor, tensor));
        return results;
    }
}