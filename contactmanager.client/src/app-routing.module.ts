import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth-guard';
import { ContactListComponent } from './app/contact-list/contact-list.component';
import { ContactDetailsComponent } from './app/contact-details/contact-details.component';
import { UpdatePasswordComponent } from './app/update-password/update-password.component';
import { LoginPageComponent } from './app/login-page/login-page.component';
import { ContactAddComponent } from './app/contact-add/contact-add.component';

const routes: Routes = [
  { path: '', redirectTo: '/contacts', pathMatch: 'full' },
  { path: 'contacts', component: ContactListComponent },
  { path: 'contact/:id', component: ContactDetailsComponent, canActivate: [AuthGuard] },
  { path: 'contactadd', component: ContactAddComponent, canActivate: [AuthGuard] },
  { path: 'contact/:id/updatepassword', component: UpdatePasswordComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginPageComponent },
  { path: '*', component: ContactListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
