import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRegistrationDto, UserLoginDto } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5000/api'; // Update with your backend URL

  constructor(private http: HttpClient) { }

  // Crop Recommendation
  getCropRecommendations(region: string): Observable<{ crops: string[], success: boolean }> {
    return this.http.get<{ crops: string[], success: boolean }>(
      `${this.baseUrl}/croprecommendation/recommend/${encodeURIComponent(region)}`
    );
  }

  // Market Price
  getCurrentPrice(crop: string): Observable<{ Crop: string, Price: number, Currency: string, LastUpdated: Date }> {
    return this.http.get<{ Crop: string, Price: number, Currency: string, LastUpdated: Date }>(
      `${this.baseUrl}/market-prices/${encodeURIComponent(crop)}`
    );
  }

  getPriceTrend(crop: string, days: number): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/market-prices/trend/${encodeURIComponent(crop)}/${days}`
    );
  }

  // Weather Report
  getCurrentWeather(region: string): Observable<{
    temperature: number;
    humidity: number;
    conditions: string;
    windSpeed: number;
  }> {
    return this.http.get<{
      temperature: number;
      humidity: number;
      conditions: string;
      windSpeed: number;
    }>(`${this.baseUrl}/weather/${encodeURIComponent(region)}`);
  }

  getWeatherForecast(region: string, days: number): Observable<{
    Region: string;
    Days: number;
    Message: string;
  }> {
    return this.http.get<{
      Region: string;
      Days: number;
      Message: string;
    }>(`${this.baseUrl}/weather/forecast/${encodeURIComponent(region)}/${days}`);
  }

  // Disease Detection
  detectDisease(plantType: string, imageFile: File): Observable<{
    PlantType: string;
    DetectionResult: string;
    Timestamp: string;
  }> {
    const formData = new FormData();
    formData.append('PlantType', plantType);
    formData.append('ImageFile', imageFile);
    
    return this.http.post<{
      PlantType: string;
      DetectionResult: string;
      Timestamp: string;
    }>(`${this.baseUrl}/disease-detection/detect`, formData);
  }

  // User Registration
  register(userDto: UserRegistrationDto): Observable<{ Message: string }> {
    return this.http.post<{ Message: string }>(`${this.baseUrl}/auth/register`, userDto);
  }

  // User Login
  login(userDto: UserLoginDto): Observable<{ Token: string }> {
    return this.http.post<{ Token: string }>(`${this.baseUrl}/auth/login`, userDto);
  }
}
