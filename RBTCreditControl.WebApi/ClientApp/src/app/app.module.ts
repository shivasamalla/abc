import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

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

import { ChangePasswordComponent } from './global/changePassword/change-password.component';
import { PageNotFoundComponent } from './global/pagenotfound/PageNotFound.Component';
import { UnauthorisedAccessComponent } from './global/unauthorized/UnauthorisedAccess.Component';

import { CustomFormsModule } from 'ng4-validators';
import { AppRoutingModule } from './app-routing.module';

import { DatepickerModule, BsDatepickerModule} from 'ngx-bootstrap/datepicker';

import { NgxEditorModule } from 'ngx-editor';

import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';

import { NgxSpinnerModule } from 'ngx-spinner';
import { GlobalSettings } from './Services/GlobalSettings';

@NgModule({
  declarations: [
    AppComponent, HomeComponent, NavMenuComponent,
    UserLoggIn, 
    AdminComponent, LocationComponent, CreateSupervisorUserComponent, EditSupervisorUserComponent,
    SupervisorComponent, CreateUserComponent, EditUserComponent,
    CorporatesComponent, CreateCorporatesComponent,
    UserCorporatesComponent,
    ReportsComponent,
    CallingUserComponent, PromiseComponent, FollowupComponent, OtherCorpStatusComponent,
    PageNotFoundComponent, UnauthorisedAccessComponent, ChangePasswordComponent
  ],
  imports: [
   
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule, HttpModule, CustomFormsModule, 
    FormsModule,
    AppRoutingModule,
    BrowserModule,
    NgxEditorModule,
    BsDatepickerModule.forRoot(),
    DatepickerModule.forRoot(),
    CommonModule,
    NgxSpinnerModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot() // ToastrModule added
  ],
  providers: [GlobalSettings],
  bootstrap: [AppComponent]
})
export class AppModule { }
