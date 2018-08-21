import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { ajaxPost } from 'rxjs/observable/dom/AjaxObservable';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls:['home.component.scss']

})
export class HomeComponent {
    empCode: any ;
    password: any ;
    userRole = "";
    invalidLogin = "";
    constructor(private _http: Http, private _router: Router, private _globalService: GlobalSettings) { }
  ngOnInit() {
    localStorage.clear();
        //this.router.navigate(['home/user']);
    }
  login() {   
        debugger;        
            this._http.get(this._globalService.hostPath + '/api/User/Login?empCode=' + this.empCode + '&password=' + this.password)
                .subscribe(result => {
                    debugger;
                    console.log('home '+result.json());
                  var resultData = result.json();
                  localStorage.clear();
         
                    if (result.status == 200 && resultData.cPwdF == false) { 
                            localStorage.setItem('role', resultData.userType);
                            localStorage.setItem('key', resultData.token);// session key
                            localStorage.setItem('id', resultData.id);
                            localStorage.setItem('name', resultData.name);
                           
                      if (resultData.userType == 'Supervisor') {
                        this._router.navigate(['user/usercorporates'], { queryParams: { key: resultData.token, role: resultData.userType } });
                        }
                        else if (resultData.userType == 'Admin') {
                            this._router.navigate(['user/admin'], { queryParams: { key: resultData.token } });
                        }
                        else if (resultData.userType == "BackOffice") {
                            this._router.navigate(['/user/usercorporates'], { queryParams: { key: resultData.token } });
                        }
                        else {
                            this._router.navigate(['/user/callinguser'], { queryParams: { key: resultData.token } });
                        }                   
                    } else if (result.status == 200 && resultData.cPwdF == true) {                     
                      debugger;
                      //alert('pwd change');
                      //this.location.replaceState('/');
                      window.location.href = this._globalService.hostPath + "/home/ChangePassword?id="+resultData.id;
                        //this._router.navigate(['/Home/ChangePassword'], { queryParams: { id: resultData.id } });
                    }else {
                        this.invalidLogin = "Invalid user name (or) password";                   
                    }
              }, error => {
                if (error.status == 401) {
                  this._router.navigate(['/UnauthorisedAccess']);
                }
                    localStorage.clear();
                    console.error(error);
                });
       
    }
}
