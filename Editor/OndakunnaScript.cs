#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace Ondakkuka
{
    internal static class OndakunnaScript
    {
        [MenuItem("Moding/AndroidBuild")]
        private static void AndroidOndakk()
        {
            Ondakk(BuildTarget.Android);
        }
        [MenuItem("Moding/WindowsBuild")]
        private static void WindowsOndakk()
        {
            Ondakk(BuildTarget.StandaloneWindows);
        }
        [MenuItem("Moding/AndroidBuild - Clean")]
        private static void AndroidVrithikkOndakk()
        {
            VrithikkOndakk(BuildTarget.Android);
        }
        [MenuItem("Moding/WindowsBuild - Clean")]
        private static void WindowsVrithikkOndakk()
        {
            VrithikkOndakk(BuildTarget.StandaloneWindows);
        }


        private static void Ondakk(BuildTarget buildTarget)
        {
            string assetBundleDirectory = $"Assets/Mods/{(buildTarget == BuildTarget.Android ? "Android" : "Windows")}";
            if (!Directory.Exists(assetBundleDirectory))
                Directory.CreateDirectory(assetBundleDirectory);

            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                            BuildAssetBundleOptions.ChunkBasedCompression,
                                            buildTarget);


            PettiyilAkkuka(assetBundleDirectory);
        }
        private static void VrithikkOndakk(BuildTarget buildTarget)
        {
            string assetBundleDirectory = $"Assets/Mods/{(buildTarget == BuildTarget.Android ? "Android" : "Windows")}";
            if (!Directory.Exists(assetBundleDirectory))
                Directory.CreateDirectory(assetBundleDirectory);

            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                            BuildAssetBundleOptions.ForceRebuildAssetBundle & BuildAssetBundleOptions.ChunkBasedCompression,
                                            buildTarget);


            PettiyilAkkuka(assetBundleDirectory);
        }

        private static async void PettiyilAkkuka(string assetBundleDirectory)
        {
            string[] files = Directory.GetFiles(assetBundleDirectory);
            int id = Progress.Start("Creating Mod Files");
            float filesCount = files.Length;
            float counter = 0;
            foreach (var file in files)
            {
                counter++;
                Progress.Report(id, counter / filesCount);

                string extension = Path.GetExtension(file);

                if (extension == ".meta" || extension == ".manifest" || extension == ".bsk")
                    continue;

                string targetFile = file + ".bsk";

                var result = Chaavi.PettiAdakkuka(file);
                await result;

                var adachapatty = result.Result;

                if (File.Exists(targetFile))
                    File.Delete(targetFile);

                File.WriteAllBytes(targetFile, adachapatty);

            }
            Progress.Finish(id);
        }
    }
}
#endif