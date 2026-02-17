using System.Collections.Generic;

namespace LucasBaran.Bootstrap
{
    internal sealed class LoadedSceneFactory
    {
        private readonly Queue<LoadedScene> _loadedSceneQueue = new();

        public LoadedScene Get()
        {
            return _loadedSceneQueue.TryDequeue(out LoadedScene pooled_loaded_scene) ? pooled_loaded_scene : new LoadedScene();
        }

        public void Return(LoadedScene loaded_scene)
        {
            Reset(loaded_scene);
            _loadedSceneQueue.Enqueue(loaded_scene);
        }

        private void Reset(LoadedScene loaded_scene)
        {
            loaded_scene.ReferenceCount = 0;
        }
    }
}
