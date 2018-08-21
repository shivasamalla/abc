import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { GlobalSettings } from './../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';

declare var $: any;

@Component({
  templateUrl: './callinguser.component.html',
  providers: [GlobalSettings]
})
export class CallingUserComponent implements OnInit {
  datePickerConfig: any = [];
  editorConfig: any = [];
  sessionToken: any;
  corporatesData: any = [];
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;
  selectedIndex: number = 0;
  viewCorporate: any = null;
  storePage: any = [];
  Amount: any = [];
  drpActionStatusData: any = [];
  flwupActStatusSelected: any = [];
  submitCorpObj: any = [];
  successMsg = "";
  userInputData: any = {};


  constructor(private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) {
    this.editorConfig = this._globalService._editorConfigData;
    this.datePickerConfig = this._globalService._datePickerConfig;
  }
  ngOnInit() {
    this.flwupActStatusSelected = null;

    this.sessionToken = localStorage.getItem('key');
    if (this._route.snapshot.queryParams.key == this.sessionToken) {
      this.bindUserCorporatesList(this.page, this.paginationSize)
    } else {
      window.location.href = this._globalService.hostPath;
    }
    debugger;
    this._http.get(this._globalService.hostPath + '/api/CorporateStatus/GetStatus?key=' + this.sessionToken)
      .subscribe(
        result => {
          debugger;
          console.log(result.json());
          var resultData = result.json();
          this.drpActionStatusData = resultData.statusList;
        }, error => {
          debugger;
          console.error(error);
        })

  }

  bindUserCorporatesList(pageNumber: number, pageSize: number) {
    this.storePage.pageNumber = pageNumber;
    this.storePage.pageSize = pageSize;
    debugger;
    this._http.get(this._globalService.hostPath + '/api/Corporate/GetUserAssignedCorporates?key=' + this.sessionToken + '&userId=' + localStorage.getItem('id') + '&page=' + pageNumber + ' &pagesize=' + pageSize)
      .subscribe(
        result => {
          debugger;
          console.log(result.json());
          var resultData = result.json();
          this.corporatesData = resultData.results;
          this.totalRecords = resultData.totalRecords;
          this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);
          $("ul.pagination li").removeClass("active");
          $("ul.pagination #li" + pageNumber).addClass("active");
        }, error => {
          console.error(error);
        })

  }

  corporateClicked(corp: any) {
    this.viewCorporate = corp;
  }
  submitCorporate() {
    debugger;
    if (this.flwupActStatusSelected == 1 || this.flwupActStatusSelected == 8) {
      if (this.userInputData.fromDate == undefined) {
        alert('Promise Date can not empty!');
        return false;
      }
      else if (this.userInputData.amount == undefined) {
        alert('Amount can not empty!');
        return false;
      }
    } else {
      if (this.userInputData.htmlContent == undefined) {
        alert('Call Note can not empty!');
        return false;
      }
    }
    this.submitCorpObj = {
      CallNote: this.userInputData.htmlContent,
      PromiseAmount: this.userInputData.amount
    }
    if (this.flwupActStatusSelected == 1 || this.flwupActStatusSelected == 8) {
      this.submitCorpObj.PromiseDate = this.userInputData.fromDate.getMonth() + 1 + '/' + this.userInputData.fromDate.getDate() + '/' + this.userInputData.fromDate.getFullYear();
    }
    this.submitCorpObj.FK_SubmitedCorporateId = this.viewCorporate.id;
    this.submitCorpObj.FK_CorporateStatusMasterId = this.flwupActStatusSelected;
    debugger;
    this._http.post(this._globalService.hostPath + '/api/CorporateAction/PostStatus?Id=' + this.viewCorporate.id + '&key=' + this.sessionToken, this.submitCorpObj)
      .subscribe(result => {
        console.log(result.json());
        var resultData = result.json();
        this.bindUserCorporatesList(this.storePage.pageNumber, this.storePage.pageSize);
        this.successMsg = resultData.msg;

        $('#floatalert').fadeIn();

        this.userInputData = {};
        $('#callNote').val('');
        if (resultData.status == true) {
          $('#floatalert').addClass('successmsg');
        }
        else {
          $('#floatalert').addClass('errormsg');
        }
        this.flwupActStatusSelected = null;
        $('#type1').css('display', 'none');
        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();
      }, error => {
        console.error(error);
        $('#type1').css('display', 'none');
        $('#floatalert').addClass('errormsg');
        $('#floatalert').fadeIn();
        this.successMsg = error.msg;
        this.userInputData = {};
        this.flwupActStatusSelected = null;
        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();
      })
    setTimeout(function () {
      $('#floatalert').removeClass();
      $('#floatalert').fadeOut();
    }, 4000);

  }
  RefreshGrid() {
    this.bindUserCorporatesList(this.storePage.pageNumber, this.storePage.pageSize)
  }
  selectActionUI() {
    debugger;
    if (this.flwupActStatusSelected == 1 || this.flwupActStatusSelected == 8) {
      $('#type1').css('display', 'block');
    }
    else {
      $('#type1').css('display', 'none');
    }

  }
  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.page = 1;// setting to default value
    this.pageShow = 0;
    this.bindUserCorporatesList(this.page, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.selectedIndex = currPage;
    this.page = currPage;
    this.bindUserCorporatesList(currPage, this.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageShow = this.pageShow + 10;
      this.bindUserCorporatesList(this.page, this.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    if (prevPageNumber >= 0) {
      this.page = prevPageNumber + 1;
      this.pageShow = this.pageShow - 10;
      this.bindUserCorporatesList(this.pageShow + 1, this.paginationSize);
    }
  }

}
