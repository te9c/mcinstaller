namespace MCInstaller.Java
{
    public class JavaReference
    {
        public readonly string PathToJava;
        public readonly JavaVersion Version;

        internal JavaReference(string path, JavaVersion version)
        {
            PathToJava = path;
            Version = version;
        }
    }
}
