import { CanActivateFn, Router } from "@angular/router";
import { ContactService } from "../services/contact-service";
import { inject } from "@angular/core";


export const AuthGuard: CanActivateFn = (route, state) => {

  const contactService = inject(ContactService);
    const router = inject(Router);

  if (!contactService.isLoggedIn()) {
      router.navigate(['/login']);
    }

    return true;
}
