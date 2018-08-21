import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Http } from '@angular/http';
//import { BrowserModule } from '@angular/platform-browser';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';

declare var $: any;

@Component({
  templateUrl: './create-supervisor-user.component.html',
  providers: [GlobalSettings]
})
export class CreateSupervisorUserComponent implements OnInit {
  supervisorUserDetails: any = {};
  checkedLocationList: any = [];
  locationData: any = [];
  empCodeErrorMsg = "";
  successMsg = "";
  sessionToken: any = [];
  constructor(private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
  ngOnInit() {
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      window.location.href = this._globalService.hostPath;
    }
    this.supervisorUserDetails.fk_LocationId = null;
    this._http.get(this._globalService.hostPath + '/api/LocationMaster/GetALL?key=' + this.sessionToken)
      .subscribe(result => {
        console.log(result.json());
        this.locationData = result.json();
      }, error => console.error(error));
  }
  verifyEmpCode() {
    this.supervisorUserDetails.name = "";
    this._http.get(this._globalService.hostPath + '/api/IntranetUser/GetEmployee?empCode=' + this.supervisorUserDetails.empCode + '&key=' + this.sessionToken)
      .subscribe(result => {
        if (result.status == 200) {
          this.supervisorUserDetails.name = result.json().emp_name;
          // $('#emCode').css('border-left', '5px solid #42A948');
        }
        else if (result.status == 204) {
          this.empCodeErrorMsg = "Employee code does not exist";
          //   $('#emCode').css('border-left', '5px solid #ff0000');                   
        }
        //this.supervisorUserDetails.name = 'Sahasra';
      }, error => {
        console.error(error);
      });
  }
  createRBTCreditSupervisor() {
    debugger;
    var LocationsSelected = new Array();
    if (this.checkedLocationList != undefined) {
      for (var i = 0; i < this.checkedLocationList.length; i++) {
        LocationsSelected.push({ 'FK_LocationId': this.checkedLocationList[i] });
      }
    }
    this.supervisorUserDetails.lstUserLocation = LocationsSelected;
    if (this.supervisorUserDetails.lstUserLocation.length > 1) {
      this.supervisorUserDetails.name = this.supervisorUserDetails.name.trim();
      this.supervisorUserDetails.empCode = this.supervisorUserDetails.empCode.trim();
      this.supervisorUserDetails.userType = "Supervisor";
      if (localStorage.getItem('id') != null) {
        this.supervisorUserDetails.CreatedBy = localStorage.getItem('id');
        this._http.post(this._globalService.hostPath + '/api/User/CreateUser?key=' + this.sessionToken, this.supervisorUserDetails)
          .subscribe(result => {
            $('#floatalert').fadeIn();
            debugger;
            if (result.json().msg == 'Already Exists!') {
              debugger;
              $('#floatalert').addClass('errormsg');
              this.successMsg = result.json().msg;
            } else {
              debugger;
              $('#floatalert').addClass('successmsg');
              this.successMsg = "Record Created Successfully";
            }
            //this.successMsg = result.json().msg;
          }, error => {
            debugger;
            $('#floatalert').addClass('errormsg');
            this.successMsg = "Failed in Creating the record";
            console.error(error);
          });
      } else {
        debugger;
        $('#floatalert').addClass('errormsg');
        this.successMsg = "Record Creation Failed, Contact Riya Support";
        // this._router.navigate(['/home']);
      }
    }
    else {
      alert('Please select atleast one branch');
    }

    setTimeout(function () {
      $('#floatalert').removeClass();
      $('#floatalert').fadeOut();
    }, 8000);

  }

  locationsSelected(checkbxId: number, event: any) {
    debugger;
    if (event.target.checked) {
      if (!this.checkedLocationList.includes(checkbxId)) {
        this.checkedLocationList.push(checkbxId);
      }
    }
    else {
      this.checkedLocationList.splice(this.checkedLocationList.indexOf(checkbxId), 1);
    }
  }
}   
