using System.Collections.Generic;
using UnityEngine;

namespace CatapultTraining
{
    public class Spawner : MonoBehaviour
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

            for (int i = 1; i <= nbCatapultsX; i++)
            {
                for (int j = 1; j <= nbCatapultsY; j++)
                {
                    GameObject cata = Instantiate(catapultPrefab, transform);
                    cata.GetComponent<CatapultAgent>().GiveBrain(brain.GetComponent<Brain>());
                    cata.transform.localPosition = new Vector3(i * XSpace, j * YSpace, 0);
                    catapults.Add(cata);
                }
            }
        }


    }
}