import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'

export interface Cheese {
  id: number,
  name: string,
  colour: string,
  image: string,
  costInCentsPerKg: number,
  published: boolean
}

@Injectable({
  providedIn: 'root'
})
export class CheeseService {

  constructor(private http: HttpClient) { 
    this.baseurl = environment.APIRoot + 'api/cheese/';
  }

  baseurl: string;

  allpublished(): Observable<Cheese[]> {

    console.log(this.baseurl);
    return this.http.get<Cheese[]>(this.baseurl);
  }

  get(id: number): Observable<Cheese> {
    return this.http.get<Cheese>(this.baseurl + id);
  }
}
