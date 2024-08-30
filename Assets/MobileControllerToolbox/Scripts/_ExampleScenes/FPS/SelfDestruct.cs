using System.Collections;
using UnityEngine;


namespace MobileController
{
    public class SelfDestruct : MonoBehaviour
    {
        [Header("Behaviour")]
        public float delay = 3.0f;


        /**/


        private void Awake()
        {
            // call self destruction
            StartCoroutine(DestroyObjectDelayed());
        }


        private IEnumerator DestroyObjectDelayed()
        {
            yield return new WaitForSeconds(delay);
            Destroy(this.gameObject);
        }
    }
}