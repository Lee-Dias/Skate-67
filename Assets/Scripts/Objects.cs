using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Scriptable Objects/Objects")]
public class Objects : ScriptableObject
{
    public enum objectType{Grind, JumpOver, Manual}
    public enum buttons { A, B, C, D, E, F, G, H  }

    [SerializeField]
    private string name;

    [SerializeField]
    private objectType type;

    [SerializeField]
    private GameObject model;

    public GameObject Model => model;
}
