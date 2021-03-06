using System;
using System.Collections.Generic;

namespace Observer
{

    interface Subject
    {
         public void registerObserver(Observer observer);
         public void removeObserver(Observer observer);
         public void notifyObservers();
    }

    interface Observer
    {
        public void update(float temperature, float humidity, float pressure);
    }

    class WeatherData : Subject
    {
        private List<Observer> observers = new List<Observer>();
        private float temperature = 17;
        private float pressure = 770;
        private float humidity = 67;

        public void registerObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void removeObserver(Observer observer)
        {
            int index = observers.IndexOf(observer);
            if (index >= 0)
            {
                observers.RemoveAt(index);
            }
        }

        public void notifyObservers()
        {
            foreach (Observer observer in observers)
            {
                observer.update(getTemperature(), getHumidity(), getPressure());
            }
        }

        float getTemperature()
        {
            return temperature;
        }

        float getHumidity()
        {
            return humidity;
        }

        float getPressure()
        {
            return pressure;
        }

        public void measurementsChanged()
        {
            notifyObservers();
        }
    }



    class CurrentConditionsDisplay : Observer
    {
        private float temperature;
        private float pressure;
        private float humidity;

        public CurrentConditionsDisplay(Subject subject)
        {
            subject.registerObserver(this);
        }


        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            display();
        }

        public void display()
        {
            Console.WriteLine("Temperature: " + temperature);
            Console.WriteLine("Humidity: " + humidity);
            Console.WriteLine("Pressure: " + pressure);
        }
     }

    class StatisticsDisplay : Observer
    {
        private float temperature;
        private float pressure;
        private float humidity;

        private float normal_temperature = 18;
        private float normal_pressure = 760;
        private float normal_humidity = 50;

        public StatisticsDisplay(WeatherData weatherData)
        {
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            display();
        }
        public void display()
        {
            if (temperature > normal_temperature)
            {
                Console.WriteLine("The temperature is above normal at " + (temperature - normal_temperature) + " C");
            }
            else
            {
                Console.WriteLine("The temperature is below normal at " + (normal_temperature - temperature) + " C"); 
            }
            if (humidity > normal_humidity)
            {
                Console.WriteLine("The humidity is above normal at " + (humidity - normal_humidity) + "%");
            }
            else
            {
                Console.WriteLine("The humidity is below normal at " + (normal_humidity - humidity) + "%");
            }
            if (pressure > normal_pressure)
            {
                Console.WriteLine("The pressure is above normal at " + (pressure - normal_pressure) + " Millimeters of mercury");
            }
            else
            {
                Console.WriteLine("The pressure is below normal at " + (normal_pressure - pressure) + " Millimeters of mercury");
            }
        }
    }

    class ForecastDisplay : Observer
    {

        private float temperature;
        private float pressure;
        private float humidity;

        public ForecastDisplay(WeatherData weatherData)
        {
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            display();
        }

        public void display()
        {
            Console.WriteLine("Temperature_will_be: " + (temperature+5) + " C");
            Console.WriteLine("Pressure_will_be: " + (pressure-10) + " Millimeters of mercury");
            Console.WriteLine("Humidity_will_be: " + (humidity+7) + "%");
        }
    }

        

    class Program
    {
        static void Main(string[] args)
        {
            var weatherData = new WeatherData();

            var currentConditionsDisplay = new CurrentConditionsDisplay(weatherData);
            var statisticsDisplay = new StatisticsDisplay(weatherData);
            var forecastDisplay = new ForecastDisplay(weatherData);

            weatherData.measurementsChanged();
        } 
    }
}
