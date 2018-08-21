import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app/app.component';
import { NavMenuComponent } from './global/navmenu/navmenu.component';
import { HomeComponent } from './global/home/home.component';
import { AdminComponent } from './Admin/admin.component';
import { CreateSupervisorUserComponent } from './Admin/createsupervisoruser/create-supervisor-user.component';
import { EditSupervisorUserComponent } from './Admin/editsupervisoruser/edit-supervisor-user.component';
import { LocationComponent } from './Admin/location/location.component';
import { UserLoggIn } from './global/UserLogInUI/user-loggin.component';

import { SupervisorComponent } from './Supervisor/supervisor.component';
import { CreateUserComponent } from './Supervisor/createuser/create-user.component';
import { EditUserComponent } from './Supervisor/edituser/edit-user.component';

import { CorporatesComponent } from './Supervisor/RBTcorporates/corporates.component';
import { CreateCorporatesComponent } from './Supervisor/RBTcorporates/create_corporates/create-corporates.component';

import { UserCorporatesComponent } from './usercorporates/user-corporates.componet';
import { ReportsComponent } from './reports/reports.components';

import { CallingUserComponent } from './calling/callinguser/callinguser.component';
import { PromiseComponent } from './calling/Promise/promise.component';
import { FollowupComponent } from './calling/followup/followup.component';
import { OtherCorpStatusComponent } from './calling/otherStatus/otherCorporateStatus.component';

import { PageNotFoundComponent } from './global/pagenotfound/PageNotFound.Component';
import { UnauthorisedAccessComponent } from './global/unauthorized/UnauthorisedAccess.Component';
import { ChangePasswordComponent } from './global/changePassword/change-password.component';

const appRoutes: Routes = [
  //{ path: 'home', component: HomeComponent },
  {
    path: 'user', component: UserLoggIn,
    children: [
      { path: 'admin', component: AdminComponent },
      { path: 'editsupervisor', component: EditSupervisorUserComponent },
      { path: 'createsupervisor', component: CreateSupervisorUserComponent },
      { path: 'location', component: LocationComponent },
      { path: 'usercreation', component: SupervisorComponent },
      { path: 'createuser', component: CreateUserComponent },
      { path: 'edituser', component: EditUserComponent },
      { path: 'rbtcorporates', component: CorporatesComponent },
      { path: 'createcorporate', component: CreateCorporatesComponent },
      { path: 'usercorporates', component: UserCorporatesComponent },
      { path: 'reports', component: ReportsComponent },
      { path: 'callinguser', component: CallingUserComponent },
      { path: 'promise', component: PromiseComponent },
      { path: 'followup', component: FollowupComponent },
      { path: 'otherCorpStatus', component: OtherCorpStatusComponent },
      { path: 'changepassword', component: ChangePasswordComponent },
      { path: 'UnauthorisedAccess', component: UnauthorisedAccessComponent },
      { path: '**', component: PageNotFoundComponent }
    ]
  },
  { path: 'UnauthorisedAccess', component: UnauthorisedAccessComponent },
    //{ path: 'home', component: HomeComponent },
  //{ path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '', component: HomeComponent, pathMatch:'full' }
  //{ path: '**', component: PageNotFoundComponent }

];
@NgModule({
    imports: [
    RouterModule.forRoot(
      appRoutes,
      /*{ enableTracing: true }*/ // <-- debugging purposes only
    )
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
