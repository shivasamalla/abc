import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Http, RequestOptions } from '@angular/http';
//import { BrowserModule } from '@angular/platform-browser';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any;

@Component({
  templateUrl: './create-user.component.html',
  providers: [GlobalSettings]
})
export class CreateUserComponent implements OnInit {

  userDetails: any = {
    lstUserCorporate: {}
  };
  checkedCorporateList: any = [];
  empCodeErrorMsg = "";
  corporateData: any = [];
  successMsg = "";
  sessionToken: any;
  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService, private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
  ngOnInit() {
    this.userDetails.userType = null;
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      window.location.href = this._globalService.hostPath;
    }

    //this.userDetails.fk_LocationId = null;
    this.userDetails.userType = null;
    this.getCorporateList();
  }
  verifyEmpCode() {
    this.userDetails.name = "";
    this.empCodeErrorMsg = "";
    this.spinner.show();
    this._http.get(this._globalService.hostPath + '/api/IntranetUser/GetEmployee?empCode=' + this.userDetails.empCode + '&key=' + this.sessionToken)
      .subscribe(result => {
        this.spinner.hide();
        if (result.status == 200) {
          this.userDetails.name = result.json().emp_name;
          //$('#emCode').css('border-left', '5px solid #42A948');
        }
        else if (result.status == 204) {
          this.empCodeErrorMsg = "Employee code does not exist in Employee Portal"
          //$('#emCode').css('border-left', '5px solid #ff0000');                   
        }
        //this.userDetails.name = 'Sahasra';
      }, error => {
        this.spinner.hide();
        console.error(error);
      });
  }
  createRBTCreditUser() {
    if (this.userDetails.lstUserCorporate.length >= 1) {
      if (this.userDetails.userType == 1) {
        this.userDetails.userType = "BackOffice";
      }
      else {
        this.userDetails.userType = "Calling";
      }
      this.userDetails.CreatedBy = localStorage.getItem('id');
      this.userDetails.fk_SupervisorId = localStorage.getItem('id');
      this.spinner.show();
      this._http.post(this._globalService.hostPath + '/api/User/CreateUser?key=' + this.sessionToken, this.userDetails)
        .subscribe(result => {
          this.spinner.hide();
          if (result.json().status = true && result.json().msg == 'Already Exists!') {
            //$('#floatalert').addClass('errormsg');
            //$('#floatalert').fadeIn();
            this.toastr.error(this.successMsg);
            this.successMsg = 'User ' + result.json().msg;
            this.userDetails.userType = null;
          }
          else {
            //$('#floatalert').addClass('successmsg');
            //$('#floatalert').fadeIn();

            this.successMsg = "User Created Successfully";
            this.toastr.success(this.successMsg);
            this.userDetails = {};
            //this.userDetails.userType = null;
            alert('User Created Succesfully');
            this._router.navigate(['/home/index/usercreation'], { queryParams: { key: this.sessionToken } });
          }
        }, error => {
          this.spinner.hide();
         // $('#floatalert').addClass('errormsg');
          //$('#floatalert').fadeIn();
          this.successMsg = "Failed in Creating the record";
          this.toastr.error(this.successMsg);
          console.error(error);
        });
      //setTimeout(function () {
      //  $('#floatalert').removeClass();
      //  $('#floatalert').fadeOut();
      //}, 8000);
    }
    else {
      alert("Please Select atleast one Corporate");
    }

  }
  getCorporateList() {
    this.spinner.show();
    this._http.get(this._globalService.hostPath + '/api/Corporate/GetCorporateByUser?key=' + this.sessionToken + '&userId=' + localStorage.getItem('id'))
      .subscribe(result => {
        this.spinner.hide();
        debugger;
        this.corporateData = result.json().resp1;
        console.log(result.json().results.resp1);
      }, error => {
        this.spinner.hide();
        console.error(error);
      })
  }
  corporatesSelected(checkbxId: number, event: any) {
    debugger;
    if (event.target.checked) {
      if (!this.checkedCorporateList.includes(checkbxId)) {
        this.checkedCorporateList.push(checkbxId);
      }
      $(".userCorpcheckbox_" + checkbxId).addClass('selected');
    }
    else {
      this.checkedCorporateList.splice(this.checkedCorporateList.indexOf(checkbxId), 1);
      $(".userCorpcheckbox_" + checkbxId).removeClass('selected');
    }
    var corpSelected = new Array();
    if (this.checkedCorporateList != undefined) {
      for (var i = 0; i < this.checkedCorporateList.length; i++) {
        corpSelected.push({ 'fk_CorporateId': this.checkedCorporateList[i] });
      }
    }
    this.userDetails.lstUserCorporate = corpSelected;
  }
}   
