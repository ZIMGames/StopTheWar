using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouKo
{
    public static class UtilsClass
    {
        public static Color GetColorWithMask(Color color, Color mask)
        {
            return new Color(color.r, color.g, color.b, mask.a);
        }

        public static Color PickRandomColor()
        {
            float f1 = Random.Range(0f, 1f);
            float f2 = Random.Range(0f, 1f);

            int variant = Random.Range(0, 3);
            if (variant == 0)
            {
                return new Color(f1, f2, 0);
            }
            else if (variant == 1)
            {
                return new Color(f1, 0, f2);
            }
            return new Color(0, f1, f2);
        }


        public static List<T> PickRandomValues<T>(List<T> valueList, int valuesNum)
        {
            List<int> pickedIndices = PickRandomIndices(valueList.Count, valuesNum);
            List<T> pickedValues = new List<T>();
            for (int i = 0; i < pickedIndices.Count; i++)
            {
                int index = pickedIndices[i];
                pickedValues.Add(valueList[index]);
            }
            return pickedValues;
        }

        public static List<int> PickRandomIndices(int listCount, int valuesNum)
        {
            List<int> freeIndices = new List<int>();
            for (int i = 0; i < listCount; i++)
            {
                freeIndices.Add(i);
            }
            int timesToRemove = listCount - valuesNum;
            while (timesToRemove-- > 0)
            {
                int indexToRemove = Random.Range(0, freeIndices.Count);
                freeIndices.RemoveAt(indexToRemove);
            }

            return freeIndices;
        }

        public static void Shuffle<T>(ref List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int i = Random.Range(0, n--);
                T temp = list[n];
                list[n] = list[i];
                list[i] = temp;
            }
        }
    }
}