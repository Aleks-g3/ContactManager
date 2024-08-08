import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ContactService } from "../services/contact-service";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public contactService: ContactService) { }
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.contactService.isLoggedIn()) {
      let newRequest = request.clone({
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${this.contactService.getToken()}`,
        }),
      });
      return next.handle(newRequest);
    }
    let newRequest = request.clone({
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
    });
    return next.handle(newRequest);
  }
}
