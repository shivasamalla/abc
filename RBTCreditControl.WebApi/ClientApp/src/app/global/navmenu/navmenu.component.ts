import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
declare var $: any;

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.scss'],
    providers: [GlobalSettings]
})
export class NavMenuComponent implements OnInit {
    userRole:any =[];
   
    sessionToken: any = [];
    constructor(private _router: Router, private _http: Http, private _globalService: GlobalSettings) { }
    ngOnInit() {
        debugger;
  
        if (localStorage.getItem('role') != null) {
            this.userRole = localStorage.getItem('role');
        }
        if (localStorage.getItem('key') != null) {
            this.sessionToken = localStorage.getItem('key');
        }
        this.updateFollowupBlink();        
    }   
    updateFollowupBlink() {
        this._http.get(this._globalService.hostPath + '/api/FollowUp/GetFollowupFlag?key=' + this.sessionToken)
            .subscribe(result => {
                debugger;
                console.log(result.json());
                var resultData = result.json();
                if (resultData.status == true) {
                    $('.follow_up').addClass('blink');
                }
                else {
                    $('.follow_up').removeClass('blink');                   
                }             

            }, error => {
                console.error(error);
            })
    }
    routerLink(path: string) {
        if (localStorage.getItem('key') != null) {
            this.sessionToken = localStorage.getItem('key');
        }
        this._router.navigate([path], { queryParams: { key: this.sessionToken } });

    }


}
