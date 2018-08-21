import { Component, OnInit } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { Route, ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
declare var $: any;

@Component({
  templateUrl: './edit-user.component.html',
  providers: [GlobalSettings]
})

export class EditUserComponent implements OnInit {
  userDetails: any = {
    lstUserCorporate: {}
  };
  corporatesData: any;
  successMsg = "";
  sessionToken: any;
  checkedCorporateList: any = [];
  userInfo: any = [];
  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService, private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }
  ngAfterViewChecked() {
    if (this.checkedCorporateList.length > 0) {
      for (var i = 0; i < this.checkedCorporateList.length; i++) {
        $('#userCorpcheckbox_' + this.checkedCorporateList[i]).attr('checked', true);
      }
    }
  }
  ngOnInit() {
    debugger;
    this.userInfo.name = this._route.snapshot.queryParams.name;
    this.userInfo.empCode = this._route.snapshot.queryParams.code;
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    if (this._route.snapshot.queryParams.key != this.sessionToken) {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }
    //to get all corporates for branch
    this.spinner.show();
    this._http.get(this._globalService.hostPath + '/api/Corporate/GetCorporateByUser?key=' + this.sessionToken + '&userId=' + localStorage.getItem('id'))
      .subscribe(result => {
        this.spinner.hide();
        debugger;
        this.corporatesData = result.json().resp1;
        console.log(this.corporatesData);
      }, error => {
        this.spinner.hide();
        console.error(error);
      })

    let id = this._route.snapshot.queryParams.id;//to get edituser Id

    this.spinner.show();
    //to get the corporates assigned to the user
    this._http.get(this._globalService.hostPath + '/api/Corporate/GetAssignedCorporate_ByUserId?key=' + this.sessionToken + '&userId=' + id)
      .subscribe(result => {
        debugger;
        this.spinner.hide();
        this.userDetails = result.json();
        console.log(result.json());
        if (this.userDetails.length > 0) {
          for (var i = 0; i < this.userDetails.length; i++) {
            if (!this.checkedCorporateList.includes(this.userDetails[i].id)) {
              this.checkedCorporateList.push(this.userDetails[i].id)
            }
          }
        }
        //if (this.checkedCorporateList.length > 0) {
        //    for (var i = 0; i < this.checkedCorporateList.length; i++) {
        //        $('#userCorpcheckbox_' + this.checkedCorporateList[i]).attr('checked', true);
        //    }
        //}

      }, error => {
        this.spinner.hide();
        console.error(error);
      });
  }

  //for edit user
  editRBTCreditUser() {
    debugger;
    var corpSelected = new Array();
    if (this.checkedCorporateList != undefined) {
      for (var i = 0; i < this.checkedCorporateList.length; i++) {
        corpSelected.push({ 'fk_CorporateId': this.checkedCorporateList[i] });

      }
    }
    this.userDetails.lstUserCorporate = corpSelected;
    if (this.userDetails.lstUserCorporate.length >= 1) {
      this.spinner.show();
      this._http.post(this._globalService.hostPath + '/api/UserCorporatePermission/UpdateUserCorporates?id=' + this._route.snapshot.queryParams.id + '&key=' + this.sessionToken, this.userDetails.lstUserCorporate)
        .subscribe(result => {
          this.spinner.hide();
          //$('#floatalert').fadeIn();
          var resultData = result.json();
          this.successMsg = resultData.msg;
          if (resultData.status == true) {
           // $('#floatalert').addClass('successmsg');
            this.toastr.success(this.successMsg);
          }
          else {
           // $('#floatalert').addClass('errormsg');
            this.toastr.error(this.successMsg);
          }         
        }, error => {
          this.spinner.hide();
         // $('#floatalert').addClass('errormsg');
         // $('#floatalert').fadeIn();
          this.successMsg = error.msg;
          this.toastr.error(this.successMsg);
          console.error(error);
          console.log(error.msgDetails)
        })
      //setTimeout(function () {
      //  $('#floatalert').removeClass();
      //  $('#floatalert').fadeOut();
      //}, 8000);
    }
    else {
      alert("Please Select atleast one Corporate");
    }
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
  }
}
