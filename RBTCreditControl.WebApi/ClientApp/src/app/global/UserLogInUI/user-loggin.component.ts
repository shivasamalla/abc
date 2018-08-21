import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
    templateUrl: 'user-loggin.component.html',
    providers: [GlobalSettings]
})
export class UserLoggIn implements OnInit {
    UserName: any = [];
    sessionToken: any = [];
    userName: any = [];
  constructor(private _http: Http, private _globalService: GlobalSettings, private urlRoute: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        if (localStorage.getItem('key') != null) {
            this.sessionToken = localStorage.getItem('key');
        }
        let urlKey = this.urlRoute.snapshot.queryParams.key;
        this.userName = localStorage.getItem('name');

        if (urlKey != this.sessionToken) {
            this.router.navigate(['/']);
        }          
       
        if (localStorage.getItem('role') != null) {
            this.userName = localStorage.getItem('name');
        }       
    }

    logoutCall() {
       
        if (localStorage.getItem('key') != null) {
            this.sessionToken = localStorage.getItem('key');
        }
      this._http.get(this._globalService.hostPath + '/api/User/LogOut?key=' + this.sessionToken)
            .subscribe(result => {                    
                if (result.json().status == true) {
                    localStorage.clear();
                    this.router.navigate(['/']);
                }
            }, error=>{
                console.error(error);
            })
    }

}
