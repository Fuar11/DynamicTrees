using DynamicTrees.Utilities;

namespace DynamicTrees.DynamicTreesComponent;

[RegisterTypeInIl2Cpp]
internal class DynamicTreeData : MonoBehaviour
{

    //WeatherStage previousWeather;
    WeatherStage currentWeather;
    WindStrength currentWind;

    public float clearAccumulation = 0; //0cm
    public float lowestAccumulation = 10; //5cm
    public float lowAccumulation = 20; //10cm
    public float lowMediumAccumulation = 30; //15cm
    public float mediumAccumulation = 50; //25cm
    public float mediumHighAccumulation = 70; //35cm
    public float highAccumulation = 80; //45cm
    public float highestAccumulation = 90; //50cm
    public float fullAccumulation = 100; //55-60cm

    float currentAccumulation = 0;
    float accumulationAmountPerHour = 0;

    public void Awake()
    {
        currentWeather = GameManager.GetWeatherComponent().GetWeatherStage();
        currentWind = GameManager.GetWindComponent().GetStrength();
        LoadAndSaveData();
    }

    public void Start()
    {
    }

    public void Update()
    {

        currentWeather = GameManager.GetWeatherComponent().GetWeatherStage();
        currentWind = GameManager.GetWindComponent().GetStrength();

        float tODHours = GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime);
        Accumulate(tODHours);

    }



    public void Accumulate(float numHoursDelta)
    {
        accumulationAmountPerHour = CalculateAccumulationAmountPerHour();
        currentAccumulation += accumulationAmountPerHour * numHoursDelta;

        currentAccumulation = Mathf.Clamp(currentAccumulation, 0f, 100f);
    }
    public float CalculateAccumulationAmountPerHour()
    {

        float accumulationAmount = 0;

        if (IsNotSnowing())
        {
            accumulationAmount = 0;

            if (currentAccumulation >= 85f) accumulationAmount = -5f;

            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount -= 2f;
            else if (currentWind == WindStrength.Windy) accumulationAmount -= 5f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount -= 10f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount -= 20f;
        }
        else if (currentWeather == WeatherStage.LightSnow)
        {
            accumulationAmount = 3f;
            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount += 2f;
            else if (currentWind == WindStrength.Windy) accumulationAmount += 5f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount += 7f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount += 10f;
        }
        else if (currentWeather == WeatherStage.HeavySnow)
        {
            accumulationAmount = 10f;
            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount += 3f;
            else if (currentWind == WindStrength.Windy) accumulationAmount += 6f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount += 8f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount += 10f;
        }
        else if (currentWeather == WeatherStage.Blizzard)
        {
            accumulationAmount = 30f;
        }

        return accumulationAmount;
    }
    public float GetCurrentAccumulation()
    {
        return currentAccumulation;
    }
    public float GetStartingAccumulation()
    {
        if (IsNotSnowing()) return clearAccumulation;
        else if (currentWeather == WeatherStage.LightSnow) return lowestAccumulation;
        else if (currentWeather == WeatherStage.HeavySnow) return mediumAccumulation;
        else if (currentWeather == WeatherStage.Blizzard) return highAccumulation;
        else return clearAccumulation;
    }
    public bool IsNotSnowing()
    {
        if (currentWeather != WeatherStage.LightSnow && currentWeather != WeatherStage.HeavySnow && currentWeather != WeatherStage.Blizzard) return true;
        else return false;
    }
    public void LoadAndSaveData()
    {
        SaveDataManager sdm = Main.sdm;
        DynamicTreeSaveDataProxy? dataToSaveOrLoad;

        dataToSaveOrLoad = sdm.Load();

        if(dataToSaveOrLoad == null)
        {
            currentAccumulation = GetStartingAccumulation();

            dataToSaveOrLoad = new DynamicTreeSaveDataProxy();
            dataToSaveOrLoad.savedAccumulation = currentAccumulation;
        }
        else
        {
            currentAccumulation = dataToSaveOrLoad.savedAccumulation;
        }
        
        sdm.Save(dataToSaveOrLoad);
    }
    public void SaveData()
    {
        SaveDataManager sdm = Main.sdm;
        DynamicTreeSaveDataProxy? dataToSave = new DynamicTreeSaveDataProxy();

        dataToSave.savedAccumulation = currentAccumulation;
        sdm.Save(dataToSave);
    }

}
