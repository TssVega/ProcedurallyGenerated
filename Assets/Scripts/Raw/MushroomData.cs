using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MushroomData {
    // -1 is no mushroom and 0-11 are mushroom types
    public int[,] mushroomMap;

    private readonly int levelSize = 32;

    public MushroomData(MushroomGeneration data) {
        //mushroomMap = new int[levelSize, levelSize];
        mushroomMap = new int[levelSize, levelSize];
        for(int x = 0; x < levelSize; x++) {
            for(int y = 0; y < levelSize; y++) {
                mushroomMap[x, y] = data.mushroomValues[x, y];
            }
        }
        /*
        mushroomMap = new int[levelSize, levelSize];
        for(int x = 0; x < levelSize; x++) {
            for(int y = 0; y < levelSize; y++) {
                mushroomMap[x, y] = CalculateMushroomValue();
            }
        }*/
    }
    /*
    private int CalculateMushroomValue() {
        int diceRollTotal = RollFourDice();
        int mushroomValue = -1;
        switch(diceRollTotal) {
            case 5:
                // Matsutake mushroom
                mushroomValue = 6;
                break;
            case 6:
                // Morel mushroom
                mushroomValue = 10;
                break;
            case 29:
                // Black trumpet mushroom
                mushroomValue = 3;
                break;
            case 7:
                // Truffle mushroom
                mushroomValue = 1;
                break;
            case 28:
                // Reishi mushroom
                mushroomValue = 5;
                break;
            case 8:
                // Chanterelle mushroom
                mushroomValue = 4;
                break;
            case 27:
                // Fly agaric mushroom
                mushroomValue = 11;
                break;
            case 9:
                // Turkey tail mushroom
                mushroomValue = 2;
                break;
            case 26:
                // Destroying angel mushroom
                mushroomValue = 0;
                break;
            case 10:
                // Porcini mushroom
                mushroomValue = 9;
                break;
            case 25:
                // Enoki mushroom
                mushroomValue = 8;
                break;
            case 11:
                // Puffball mushroom
                mushroomValue = 7;
                break;
        }
        return mushroomValue;
    }*/
    /*
    private int RollFourDice() {
        int diceRollTotal = 0;
        // Roll 4 dice
        for(int i = 0; i < 4; i++) {
            int dieResult = Random;
            diceRollTotal += dieResult;
        }
        return diceRollTotal;
    }*/
}
