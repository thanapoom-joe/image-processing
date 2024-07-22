namespace ImageProcessingService.Models;

public class ModelInput
{
    /// <summary>
    /// The byte[] representation of the image.
    /// The model expects image data to be of this type for training.
    /// </summary>
    public byte[] Image { get; set; }
    
    /// <summary>
    /// the numerical representation of the <see cref="Label"/>.
    /// </summary>
    public UInt32 LabelAsKey { get; set; }

    /// <summary>
    /// The fully qualified path where the image is stored.
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// The category the image belongs to. This is the value to predict.
    /// </summary>
    public string Label { get; set; }
}