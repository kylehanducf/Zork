namespace Zork.Common
{
    public interface IOutputService
    {

        void Write(object obj);
        
        void Write(string message);
        
        void WriteLine(object obj);

        void WriteLine(string message);
    }
}