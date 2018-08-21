import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { Http } from '@angular/http';
import { ActivatedRoute, Router } from '@angular/router';

declare var $: any;

@Component({
    templateUrl: './change-password.component.html',
    providers: [GlobalSettings]
})
export class ChangePasswordComponent implements OnInit {
    sessionToken: any = [];
    changepwd: any = [];
    oldPassword = "";
    password = "";
    CnfrmPassword = "";
    successMsg = "";
    pwdErrMsg = "";
    constructor(private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
    ngOnInit() {
        debugger;
        this.sessionToken = localStorage.getItem('key');
        if (this.sessionToken == this._route.snapshot.queryParams.key) {

        }
        else {
            window.location.href = this._globalService.hostPath;
        }

    }
    changePassword() {
        debugger;
        if (this.CnfrmPassword == this.password && this.password != "reset@123") {
            let id = localStorage.getItem('id');
            var userdetailsObj = {
                id: id,
                Password: this.password
            }
            this._http.post(this._globalService.hostPath + '/api/UserAuthorization/ChangePassword?key=' + this.sessionToken + '&id=' + id, userdetailsObj)
                .subscribe(result => {
                    $('#floatalert').addClass('successmsg');
                    $('#floatalert').fadeIn();
                    //this.successMsg = result.json().msg;
                    alert('Password changed successfully!');
                    window.location.href = this._globalService.hostPath;
                    //console.log('change pwd' + result);
                }, error => {
                    //if (error.json().errorMsg == "Invlaid Old Password") {
                    //    this.successMsg = error.json().errorMsg;
                    //    $('#floatalert').addClass('errormsg');
                    //    $('#floatalert').fadeIn();
                    //}
                    console.error(error);
                })
        }
        else
            if (this.password == "reset@123") {
                this.password = this.CnfrmPassword = this.oldPassword = "";
                this.pwdErrMsg = "New Password can't be 'reset@123'";
            }
            //else if (this.oldPassword != "reset@123") {
            //    this.password = this.CnfrmPassword = this.oldPassword = "";
            //    this.pwdErrMsg = "Old Password not matched";
            //}
            else {
                this.password = this.CnfrmPassword = this.oldPassword = "";
                this.pwdErrMsg = "Old Password and New Password can't be same";
            }


        setTimeout(function () {
            $('#floatalert').removeClass();
            $('#floatalert').fadeOut();
        }, 4000);
    }
    hideErrorMsg() {
        this.pwdErrMsg = "";
    }
}
