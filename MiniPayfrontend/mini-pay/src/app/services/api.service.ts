import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { PaymentProvider } from '../models/payment-provider.model';
import { environment } from '../../environments/environment';
import { PaymentRequestPayload } from '../models/payment-request.model';
import { PaymentResponse } from '../models/payment-response.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  private handleError(error: HttpErrorResponse) {
    console.error('API Error:', error);
    return throwError(() => new Error(error.message || 'Server error'));
  }

  getProviders(): Observable<PaymentProvider[]> {
    return this.http.get<PaymentProvider[]>(`${this.baseUrl}/paymentproviders`, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  getProvider(id: string): Observable<PaymentProvider> {
    return this.http.get<PaymentProvider>(`${this.baseUrl}/paymentproviders/${id}`, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  createProvider(provider: PaymentProvider): Observable<PaymentProvider> {
    return this.http.post<PaymentProvider>(`${this.baseUrl}/paymentproviders`, provider, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  updateProvider(provider: PaymentProvider): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/paymentproviders/${provider.id}`, provider, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  deleteProvider(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/paymentproviders/${id}`, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  toggleProviderStatus(id: string): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/paymentproviders/${id}/toggle-active`, {}, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }

  processPayment(request: PaymentRequestPayload): Observable<PaymentResponse> {
    return this.http.post<PaymentResponse>(`${this.baseUrl}/payments`, request, {
      withCredentials: true
    }).pipe(catchError(this.handleError));
  }
}
