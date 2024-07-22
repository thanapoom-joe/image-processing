using ImageProcessingService.Interfaces;
using ImageProcessingService.Models;
using ImageProcessingService.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using Tensorflow;
using static Tensorflow.Binding;


namespace ImageProcessingService.Services;

public class FaceDetectionService : IFaceDetectionService
{
    private Session session;
    private readonly ILogger<FaceDetectionService> logger;
    private string projectDirectory;
    private string workspaceRelativePath;
    private string assetsRelativePath;

    public FaceDetectionService(
        ILogger<FaceDetectionService> logger)
    {
        this.logger = logger;
    }

    public void Initialize()
    {
        string modelPath = Path.Combine(Environment.CurrentDirectory, "save_model");

        // Load the model
        var graph = new Graph().as_default();
        this.session = tf.Session(graph);
        tf.train.import_meta_graph($"{modelPath}/saved_model.pb");
    }
    
    public void OutputPrediction(ModelOutput prediction)
    {
        string imageName = Path.GetFileName(prediction.ImagePath);
        this.logger.LogInformation($"Image: {imageName} | Actual Value: {prediction.Label} " +
                                   $"| Predicted Value: {prediction.PredictedLabel}");
    }

    public Task<object> ProcessImage(string filePath)
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