using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI{
    public class SimulationSettings : MonoBehaviour
    {
        public void changeInitialPopulation(float i){
            int n = Mathf.RoundToInt(i);
            Entities.DeerStats.initialDeerPopulation = n;

        }
        public void changeMovementSpeed(float i){
            Entities.DeerStats.casualMovementSpeed = i;

        }
        public void changeHungerRate(float i){
            Entities.DeerStats.valueChangeRate = i;

        }
        public void changeThirstRate(float i){
            
        }
        public void changeInitialNumberOfTrees(float i){
            int n = Mathf.RoundToInt(i);
            Entities.EnvironmentStats.decorativesPopulation = n;

        }
        public void changeInitialNumberOfPlants(float i){
            int n = Mathf.RoundToInt(i);
            Entities.EnvironmentStats.initialEdiblePlantPopulation = n;

        }
        public void changePlantGrowRate(float i){
            Entities.EnvironmentStats.plantGrowTime = i;

        }
        public void changeMaxPlantPopulation(float i){
            int n = Mathf.RoundToInt(i);
            Entities.EnvironmentStats.maxPlantPopulation = n;
            
        }
    }
}