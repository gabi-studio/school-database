namespace School.Models
{
    public class TeacherResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Teacher? Data { get; set; }
    }
}