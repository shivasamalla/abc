import { Component, OnInit } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

declare var element: any;
declare var angular: any;
declare var $: any;
@Component({
  templateUrl: './corporates.component.html'
})

export class CorporatesComponent implements OnInit {
  successMsg = '';
  sessionToken: any=[];
  corporatesData: any=[];
  drpdwnPageSizes = [50, 100, 200, 500];
  paginationSize = 50;

  page = 1;
  pageShow = 0;
  totalNoofpages: number = 0;
  selectedIndex: number = 0;
  totalRecords: number = 0;
  constructor(private spinner: NgxSpinnerService, private _http: Http, private _globalService: GlobalSettings, private _route: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    let urlKey = this._route.snapshot.queryParams.key;
    if (urlKey != this.sessionToken) {
      //this._router.navigate(['/home']);
      window.location.href = this._globalService.hostPath;
    }
    this.bindCorporates(this.page, this.paginationSize);
    //http://erp.riya.travel/erpvisaservice/services/GetCustomers?CompanyName=Riya%20Travel%20and%20Tours&LocationCode=BOM&SearchPattern=9764340
  }
  bindCorporates(pageNumber: number, pageSize: number) {
    this.spinner.show();

    this._http.get(this._globalService.hostPath + '/api/Corporate/GetCorporate?page=' + pageNumber + '&pagesize=' + pageSize + '&key=' + this.sessionToken + '&userId=' + localStorage.getItem('id'))
      .subscribe(result => {
        debugger;
        this.spinner.hide();
        this.corporatesData = result.json().results;
        this.totalRecords = result.json().totalRecord;
        this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);

        $("ul.pagination li").removeClass("active");
        $("ul.pagination #li" + pageNumber).addClass("active");

      }, error => {
        this.spinner.hide();
        console.error(error);
      })
  }
  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.page = 1;
    this.pageShow = 0;
    this.bindCorporates(this.page, this.paginationSize);
  }


  currentPageBind(currPage: any) {
    this.selectedIndex = currPage;
    debugger;
    this.page = currPage;
    this.bindCorporates(currPage, this.paginationSize);
    $("ul.pagination li").removeClass("active");
  }
  nextPage(nextpageNumber: number) {
    debugger;
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageShow = this.pageShow + 10;
      this.bindCorporates(this.page, this.paginationSize);

    }
  }
  previousPage(prevPageNumber: any) {
    debugger;
    if (prevPageNumber >= 0) {
      this.page = prevPageNumber + 1;
      //if (prevPageNumber >= 0) {
      this.pageShow = this.pageShow - 10;
      this.bindCorporates(this.pageShow + 1, this.paginationSize);
      //}
    }
  }

}
