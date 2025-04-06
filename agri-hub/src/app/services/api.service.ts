import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  // Other methods remain unchanged...
}