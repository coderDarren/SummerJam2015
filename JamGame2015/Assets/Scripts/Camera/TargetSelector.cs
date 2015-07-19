using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour {

    public LayerMask targetLayer;
    public GameObject ProjectorItem;
    public string SELECT = "MousePan";
    public float projectorRotationSmooth = 15f;
    public Color projectorColor = Color.green;
    public int projectorIntensity = 4;

    [System.Serializable]
    public class TargetProjector
    {
        public GameObject projector;
        public Color projectorColor;
        public Transform target;

        public TargetProjector(Transform target, GameObject projector, Color projectorColor)
        {
            this.target = target;
            this.projector = Instantiate(projector as GameObject);
            this.projectorColor = projectorColor;
            projector.transform.position = target.position + Vector3.up * 4;
            projector.GetComponent<Projector>().material.color = projectorColor;
        }

        public void UpdatePosition()
        {
            projector.transform.position = target.position + Vector3.up * 4;
        }
    }

    List<TargetProjector> projectors;
    float selectInput;

    void Start()
    {
        projectors = new List<TargetProjector>();
        selectInput = 0;
    }

    void GetInput()
    {
        selectInput = Input.GetAxisRaw(SELECT);
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        FindNewProjectors();
        TurnProjectors();
    }

    void FindNewProjectors()
    {
        if (selectInput > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, targetLayer))
            {
                RemoveProjectors();
                
                for (int i = 0; i < projectorIntensity; i++)
                {
                    TargetProjector tp = new TargetProjector(hit.collider.transform, ProjectorItem, projectorColor);
                    projectors.Add(tp);
                }
            }
        }
    }

    void TurnProjectors()
    {
        foreach (TargetProjector tp in projectors)
        {
            tp.UpdatePosition();
            tp.projector.transform.rotation *= Quaternion.AngleAxis(projectorRotationSmooth * Time.deltaTime, Vector3.forward);
        }
    }

    void RemoveProjectors()
    {
        for (int i = projectors.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(projectors[i].projector);
        }
        projectors.Clear();
    }
}
