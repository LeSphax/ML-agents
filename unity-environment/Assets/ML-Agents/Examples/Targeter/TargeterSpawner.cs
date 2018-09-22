using System.Collections.Generic;
using UnityEngine;

namespace Targeter
{
    public class TargeterSpawner : MonoBehaviour
    {

        public int nbCatapultsX;
        public int nbCatapultsY;
        public float XSpace;
        public float YSpace;

        public bool onlyOne;

        public GameObject catapultPrefab;

        private List<GameObject> catapults = new List<GameObject>();

        private void Start()
        {
#if UNITY_EDITOR
            if (onlyOne)
            {
                nbCatapultsX = 1;
                nbCatapultsY = 1;
            }
#endif
            Reset();
        }

        private void Update()
        {
            if (Input.GetKeyDown("r"))
            {
                Reset();
            }
        }

        private void Reset()
        {
            catapults.ForEach(catapult => Destroy(catapult));
            catapults.Clear();

            GameObject brain = GameObject.FindGameObjectWithTag("GameController");

            for (int i = 0; i < nbCatapultsX; i++)
            {
                for (int j = 0; j < nbCatapultsY; j++)
                {
                    GameObject cata = Instantiate(catapultPrefab, transform);
                    cata.GetComponent<AimingAgent>().GiveBrain(brain.GetComponent<Brain>());
                    cata.transform.localPosition = new Vector3(i * XSpace, j * YSpace, 0);
                    catapults.Add(cata);
                }
            }
        }


    }
}