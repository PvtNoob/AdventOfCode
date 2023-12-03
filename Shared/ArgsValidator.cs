namespace Shared {
    public static class ArgsValidator {
        public static bool IsValidArgs(string[] args) {
            if(args.Length == 0) {
                Console.Error.WriteLine("Please provide the path to the input file via command line argument.");
                return false;
            }
            if (!File.Exists(args[0])) {
                Console.Error.WriteLine("Path provided via command line argument does not exist.");
                return false;
            }
            return true;
        }
    }
}
