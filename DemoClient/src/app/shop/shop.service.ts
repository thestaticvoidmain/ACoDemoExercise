import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IPackage } from '../models/package';


@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getPackages(){
    return this.http.get<IPackage[]>(this.baseUrl + 'packages');
  }
}
