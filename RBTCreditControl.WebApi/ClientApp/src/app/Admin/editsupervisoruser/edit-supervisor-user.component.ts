import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { Route, ActivatedRoute, Router } from '@angular/router';

declare var $: any;
@Component({
  templateUrl: './edit-supervisor-user.component.html',
  providers: [GlobalSettings]
})

export class EditSupervisorUserComponent implements OnInit {
  userDetails: any = {
    lstUserLocation: {}
  };
  BranchesData: any = [];
  checkedBranchesList: any = [];
  successMsg = "";
  sessionToken: any;
  userActivate: any = [];

  constructor(private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
  ngAfterViewChecked() {
    this.userActivate = this.userDetails.isActive;
    if (this.checkedBranchesList.length > 0) {
      for (var i = 0; i < this.checkedBranchesList.length; i++) {
        $('#supervisorBranch_' + this.checkedBranchesList[i]).attr('checked', true);
      }
    }
  }
  ngOnInit() {
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }

    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }
    debugger;
    this._http.get(this._globalService.hostPath + '/api/LocationMaster/GetALL?key=' + this.sessionToken)
      .subscribe(result => {
        this.BranchesData = result.json();
      }, error => console.error(error));
    //this.userDetails.empName = this._route.snapshot.queryParams.name;
    //this.userDetails.empCode = this._route.snapshot.queryParams.code;

    this._http.get(this._globalService.hostPath + '/api/User/GetUserById?id=' + this._route.snapshot.queryParams.id + '&key=' + this.sessionToken)
      .subscribe(result => {
        debugger;
        this.userDetails = result.json().userDetails[0];
        this.userActivate = this.userDetails.isActive;

        console.log(result.json());
        if (this.userDetails.lstUserLocation.length > 0) {
          for (var i = 0; i < this.userDetails.lstUserLocation.length; i++) {
            if (!this.checkedBranchesList.includes(this.userDetails.lstUserLocation[i].fK_LocationId)) {
              this.checkedBranchesList.push(this.userDetails.lstUserLocation[i].fK_LocationId)
            }
          }
        }

      }, error => {
        console.error(error);
      });
  }

  //for edit user
  editRBTCreditSupervisor() {
    var branchesSelected = new Array();
    if (this.checkedBranchesList != undefined) {
      for (var i = 0; i < this.checkedBranchesList.length; i++) {
        branchesSelected.push({ 'fK_LocationId': this.checkedBranchesList[i] });
      }
    }
    this.userDetails.lstUserLocation = branchesSelected;
    debugger;
    if (this.userDetails.lstUserLocation.length >= 1) {
      this._http.post(this._globalService.hostPath + '/api/User/UpdateUserLocation?id=' + this.userDetails.id + '&key=' + this.sessionToken, this.userDetails)
        .subscribe(result => {
          debugger;
          $('#floatalert').fadeIn();
          $('#floatalert').addClass('successmsg');

          this.successMsg = "Edited Successfully";
        }, error => {
          debugger;
          $('#floatalert').fadeIn();
          $('#floatalert').addClass('errormsg');
          this.successMsg = "Error in Editing record";
          console.error(error);
        })
    }
    else {
      alert("Please Select atleast one Branch");
    }
    setTimeout(function () {
      $('#floatalert').removeClass();
      $('#floatalert').fadeOut();
    }, 8000);
  }


  branchesSelected(checkbxId: number, event: any) {
    debugger;
    if (event.target.checked) {
      if (!this.checkedBranchesList.includes(checkbxId)) {
        this.checkedBranchesList.push(checkbxId);
      }
    }
    else {
      this.checkedBranchesList.splice(this.checkedBranchesList.indexOf(checkbxId), 1);
    }

  }
}
