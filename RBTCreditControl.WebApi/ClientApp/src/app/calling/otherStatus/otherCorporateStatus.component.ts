import { Component, OnInit } from '@angular/core';
import { GlobalSettings } from './../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';
import { Http } from '@angular/http';

declare var $: any;
@Component({
  templateUrl: './otherCorporateStatus.component.html',
  providers: [GlobalSettings]
})
export class OtherCorpStatusComponent implements OnInit {
  datePickerConfig: any = [];
  editorConfig: any = [];
  sessionToken: any = [];
  successMsg: any = [];
  corporatesData: any = [];
  viewCorporate: any = null;
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;
  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;
  selectedIndex: number = 0;
  storePage: any = [];
  flwupActStatusSelected: any = [];
  drpActionStatusData: any = [];
  submitFlwupCorpObj: any = [];
  corpStatusHistoryData: any = [];
  userInputData: any = {};
  constructor(private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) {
    this.editorConfig = this._globalService._editorConfigData;
    this.datePickerConfig = this._globalService._datePickerConfig;
  }
  ngOnInit() {
    this.flwupActStatusSelected = null;
    this.sessionToken = localStorage.getItem('key');
    if (this._route.snapshot.queryParams.key == this.sessionToken) {
      this.bindOtherCorpStatusList(this.page, this.paginationSize);
      //api controller for status dropdown    
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

    } else {
      window.location.href = this._globalService.hostPath;
    }
  }
  bindOtherCorpStatusList(pageNumber: number, pageSize: number) {
    this.storePage.pageNumber = pageNumber;
    this.storePage.pageSize = pageSize;
    this._http.get(this._globalService.hostPath + '/api/Promise/GetPromisedCorporate?key=' + this.sessionToken + '&page=' + pageNumber + '&pagesize=' + pageSize + '&IsPromise=' + false)
      .subscribe(
        result => {
          debugger;
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
  promiseViewClicked(vData: any) {
    this.viewCorporate = vData;

    //api for binding
    this._http.get(this._globalService.hostPath + '/api/CorporateAction/GetStatusByCorpId?key=' + this.sessionToken + '&corpId=' + this.viewCorporate.fK_SubmitedCorporateId)
      .subscribe(
        result => {
          debugger;
          console.log(result.json());
          var viewData = result.json();
          this.viewCorporate = viewData.resp1[0];
          this.corpStatusHistoryData = viewData.resp1[0].lstUserCorporateAction;
        }, error => {
          console.error(error);
        })
  }
  promiseActionClicked(vData: any) {
    this.viewCorporate = vData;
  }
  RefreshGrid() {
    this.bindOtherCorpStatusList(this.storePage.pageNumber, this.storePage.pageSize)
  }
  submitCorporate() {
    debugger;
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
    this.submitFlwupCorpObj = {
      CallNote: this.userInputData.htmlContent,
      PromiseAmount: this.userInputData.amount
    }
    if (this.flwupActStatusSelected == 1 || this.flwupActStatusSelected == 8) {
      this.submitFlwupCorpObj.PromiseDate = this.userInputData.fromDate.getMonth() + 1 + '/' + this.userInputData.fromDate.getDate() + '/' + this.userInputData.fromDate.getFullYear();
    }
    this.submitFlwupCorpObj.FK_SubmitedCorporateId = this.viewCorporate.fK_SubmitedCorporateId;
    this.submitFlwupCorpObj.FK_CorporateStatusMasterId = this.flwupActStatusSelected;

    this._http.post(this._globalService.hostPath + '/api/CorporateAction/PostStatus?Id=' + this.viewCorporate.id + '&key=' + this.sessionToken, this.submitFlwupCorpObj)
      .subscribe(result => {
        console.log(result.json());
        var resultData = result.json();
        this.bindOtherCorpStatusList(this.storePage.pageNumber, this.storePage.pageSize);
        $('#floatalert').fadeIn();

        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();

        this.userInputData = {};
        this.flwupActStatusSelected = null;
        this.successMsg = resultData.msg;
        if (resultData.status == true) {
          $('#floatalert').addClass('successmsg');
        }
        else {
          $('#floatalert').addClass('errormsg');
        }
      }, error => {
        console.error(error);
        this.successMsg = error.msg;
        var popupClose = document.getElementById("popupClose") as any;
        popupClose.click();
        this.userInputData = {};
        this.flwupActStatusSelected = null;
        $('#floatalert').addClass('errormsg');
        $('#floatalert').fadeIn();
      })
    setTimeout(function () {
      $('#floatalert').removeClass();
      $('#floatalert').fadeOut();
    }, 4000);
  }
  selectActionUI() {
    debugger;
    if (this.flwupActStatusSelected == 1 || this.flwupActStatusSelected == 8) {// for promise and received
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
    this.bindOtherCorpStatusList(this.page, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.selectedIndex = currPage;
    this.page = currPage;
    this.bindOtherCorpStatusList(currPage, this.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageShow = this.pageShow + 10;
      this.bindOtherCorpStatusList(this.page, this.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    if (prevPageNumber >= 0) {
      this.page = prevPageNumber + 1;
      this.pageShow = this.pageShow - 10;
      this.bindOtherCorpStatusList(this.pageShow + 1, this.paginationSize);
    }
  }

}
