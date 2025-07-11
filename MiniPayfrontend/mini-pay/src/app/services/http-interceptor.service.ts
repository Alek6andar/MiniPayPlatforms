import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const authReq = req.clone({
    withCredentials: true
  });

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = '';

      if (error.error && typeof error.error === 'object' && 'message' in error.error) {
        errorMessage = `Client Error: ${(error.error as any).message}`;
      } else if (typeof error.error === 'string') {
        errorMessage = `Server Error: ${error.status} - ${error.error}`;
      } else {
        errorMessage = `Server Error: ${error.status} - ${error.message}`;
      }

      console.error('HTTP Error:', errorMessage);
      return throwError(() => new Error(errorMessage));
    })
  );
};

