namespace ME.ECS {

    [System.Serializable]
    public class FeaturesList {

        [System.Serializable]
        public class FeatureData {

            public bool enabled;
            public FeatureBase feature;
            public FeatureBase featureInstance;

        }

        public System.Collections.Generic.List<FeatureData> features = new System.Collections.Generic.List<FeatureData>();

        internal void Initialize(World world) {

            for (int i = 0; i < this.features.Count; ++i) {

                var item = this.features[i];
                if (item.enabled == true) {
                    
                    var instance = UnityEngine.Object.Instantiate(item.feature);
                    instance.name = item.feature.name;
                    world.AddFeature(item.featureInstance = instance);
                    
                }
                
            }

        }

        internal void DeInitialize(World world) {
            
            for (int i = 0; i < this.features.Count; ++i) {
                
                var item = this.features[i];
                if (item.enabled == true) {
                    
                    world.RemoveFeature(item.featureInstance);
                    UnityEngine.Object.DestroyImmediate(item.featureInstance);
                    item.featureInstance = null;

                }
                
            }

        }

    }

    public abstract class FeatureBase : UnityEngine.ScriptableObject, IFeatureBase {

        public World world { get; set; }
        public SystemGroup systemGroup;

        internal void DoConstruct() {
            
            this.systemGroup = new SystemGroup(this.world, this.name);
            this.OnConstruct();
            
        }

        internal void DoDeconstruct() {
            
            this.OnDeconstruct();
            
        }
        
        protected abstract void OnConstruct();
        protected abstract void OnDeconstruct();

        protected bool AddSystem<TSystem>() where TSystem : class, ISystemBase, new() {

            if (this.systemGroup.HasSystem<TSystem>() == false) {
                
                return this.systemGroup.AddSystem<TSystem>();
                
            }

            return false;

        }

        protected bool AddModule<TModule>() where TModule : class, IModuleBase, new() {

            if (this.world.HasModule<TModule>() == false) {
                
                return this.world.AddModule<TModule>();
                
            }

            return false;

        }

    }

    public abstract class Feature : FeatureBase, IFeature {

    }

}