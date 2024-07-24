using ImageProcessingService.Interfaces;

namespace ImageProcessingService.Initializers
{
    public class FaceDetectionInitializer : IAsyncInitializer
    {
        private readonly IFaceDetectionService _faceDetectionService;

        public FaceDetectionInitializer(IFaceDetectionService faceDetectionService)
        {
            _faceDetectionService = faceDetectionService;
        }

        public async Task InitializeAsync()
        {
            await this._faceDetectionService.InitializeAsync();
        }
    }
}
