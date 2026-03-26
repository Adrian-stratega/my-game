using UnityEngine;

namespace IndiGame.Phone
{
    [CreateAssetMenu(fileName = "PhotoData", menuName = "LAST DELIVERY/Photo Data")]
    public class PhotoData : ScriptableObject
    {
        public Sprite photo;        // null = foto aún no "revelada"
        public string timestamp;    // ej: "Mar 12 · 03:47 AM"
        public string caption;      // texto oculto, visible solo en visor
        public bool isRevealed;     // false por defecto
    }
}
