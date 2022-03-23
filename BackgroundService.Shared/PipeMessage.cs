using System;

namespace BackgroundService.Shared
{
    [Serializable]
    public class PipeMessage
    {
        public PipeMessage()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public ActionType Action { get; set; }
        public string? Text { get; set; }
    }
}