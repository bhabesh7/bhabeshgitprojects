const express = require('express');
const app = new express();
const request = require('request');

app.use(express.json());
app.set('view engine', 'ejs');
let city ='bangalore';
//openweathermap key - for current weather API
//Replace XXXXXXXX with your API key

app.get('/', (req,res)=>
{
    res.send('please provide city name in the url params');   
});

app.get('/:cityname', (req,res)=>
{
    city=req.params.cityname;
    
    let url =`https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=XXXXXXXX`;
        
    request(url,(error, _response,body)=>{
        const weather_json =JSON.parse(body);
        console.log(weather_json);
        
        if(weather_json.cod ==='404')
        {
            res.send('Invalid request.');
            return;
        }

        //273.15 is kelvin t celsius conversion
        var weather = {city:city,
        temperature: Math.round(weather_json.main.temp -273.15),
        description:weather_json.weather[0].description,
        icon:weather_json.weather[0].icon,
        windspeed: weather_json.wind.speed
        };
        
        res.render('weather',{weather:weather});        
    });
});

app.listen(8000,()=> console.log('listening at port 8000'));