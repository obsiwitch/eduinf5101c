using System.Collections.Generic;
using ImageSynthesis.Models;

namespace ImageSynthesis.Lights {

    class PhongIllumination : IlluminationModel {
        
        private V3 CameraPos;
        
        public PhongIllumination(V3 cameraPos) {
            CameraPos = cameraPos;
        }
        
        /// Computes the ambient component of the Phong reflection model.
        override public Color ComputeAmbientLight(
            AmbientLight aL, Object3D obj, V3 p, float u, float v
        ) {
            return obj.TextureColor(u,v) * aL.Intensity * obj.Material.KAmbient;
        }
        
        /// Computes the diffuse and specular components of the Phong
        /// reflection model.
        override public Color ComputePointLight(
            PointLight pL, Object3D obj, V3 p, float u, float v
        ) {
            V3 normalVec = obj.Normal(p);
            
            V3 incidentVec = pL.Direction(p);
            
            V3 reflectedVec = 2 * (normalVec * incidentVec) *
                              normalVec - incidentVec;
            reflectedVec.Normalize();
            
            V3 viewingVec = CameraPos - p;
            viewingVec.Normalize();
            
            // Diffuse reflection
            Color diffuseIllu = new Color(0, 0, 0);
            if (incidentVec * normalVec > 0.0f) {
                diffuseIllu = obj.TextureColor(u,v) * pL.Intensity *
                              obj.Material.KDiffuse * (incidentVec * normalVec);
            }
            
            // Specular reflection
            Color specularIllu = new Color(0, 0, 0);
            if (reflectedVec * viewingVec > 0.0f) {
                specularIllu = pL.Intensity * obj.Material.KSpecular *
                     Mathf.Pow(
                        reflectedVec * viewingVec,
                        obj.Material.Shininess
                     );
            }
            
            return diffuseIllu + specularIllu;
        }
    }

}
