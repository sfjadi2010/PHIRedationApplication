import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class FileProcessService {

  private url = environment.baseUrl + 'api/fileprocessor';
  constructor(private http: HttpClient) { }

  processFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.url}`, formData);
  }
}
