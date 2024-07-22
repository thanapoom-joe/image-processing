namespace ImageProcessingService.Models;

public class ModelOutput
{
    /// <summary>
    /// The fully qualified path where the image is stored.
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// The original category the image belongs to. This is the value to predict.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// The value predicted by the model.
    /// </summary>
    public string PredictedLabel { get; set; }
}