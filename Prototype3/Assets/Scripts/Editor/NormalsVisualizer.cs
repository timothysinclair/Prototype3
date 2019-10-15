#if false
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshFilter))]
public class NormalsVisualizer : Editor {

    private MeshFilter mf;

	void OnEnable() {
		mf = (target as MeshFilter);
    }

    void OnSceneGUI() {
        if (mf == null) {
            return;
        }

		var mesh = mf.sharedMesh;
		if (mesh == null) { return; }
		for (int i = 0; i < mesh.vertexCount; i++) {
            Handles.matrix = mf.transform.localToWorldMatrix;
            Handles.color = Color.yellow;
            Handles.DrawLine(
                mesh.vertices[i],
                mesh.vertices[i] + mesh.normals[i]);
        }
    }
}
#endif
