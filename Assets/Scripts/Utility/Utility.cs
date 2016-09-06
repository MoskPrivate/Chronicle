using System.Collections;

public static class Utility {

    public static T[] ShuffleArray<T>(T[] array,int _seed)
    {
        System.Random pseudoRandom = new System.Random(_seed);

        for (int i = 0; i < array.Length -1; i++)
        {
            int randomIndex = pseudoRandom.Next(i,array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
	
}
