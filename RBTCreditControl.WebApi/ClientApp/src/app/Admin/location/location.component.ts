import { Component, Inject, OnInit } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { GlobalSettings } from '../../Services/GlobalSettings';

declare var $: any;

@Component({
  templateUrl: './location.component.html',
  providers: [GlobalSettings]
})
export class LocationComponent implements OnInit {
  locationData: any = [];
  successMsg: string = "";
  //totalLocationCount = 0;
  options: any = [];
  sessionToken: any = [];
  drpdwnPageSizes = [10, 20, 50, 100];
  paginationSize = 50;
  page = 1;
  pageS = 0;
  totalNoofpages: number = 0;
  totalRecords: number = 0;

  constructor(private _http: Http, private _globalService: GlobalSettings) { }
  ngOnInit() {
    if (localStorage.getItem('key') != null) {
      this.sessionToken = localStorage.getItem('key');
    }
    //let headers: any = new Headers();
    //headers.append('Content-Type', 'application/json');
    //headers.append('Token', this.sessionToken);
    //this.options = new RequestOptions({ headers: headers });
    this.bindLocations(this.page, this.paginationSize);
  }
  bindLocations(pageNumber: number, pageSize: number) {
    debugger;

    this._http.get(this._globalService.hostPath + '/api/LocationMaster/GetALL_P?page=' + pageNumber + '&pageSize=' + pageSize + '&key=' + this.sessionToken)
      .subscribe(result => {
        debugger;
        this.locationData = result.json().resp;
        this.totalRecords = result.json().totalRecords;
        this.totalNoofpages = Math.ceil(this.totalRecords / this.paginationSize);
        console.log(result.json());
      }, error => {
        console.error(error);
      });
  }
  deleteLocation(LocationId: number) {
    debugger;
    if (confirm("Do you want to delete Company!")) {
      this._http.delete(this._globalService.hostPath + '/api/LocationMaster/DeleteLocationMaster?id=' + LocationId + '&key=' + this.sessionToken)
        .subscribe(result => {
          // $('#loc_' + LocationId).remove();
          this.bindLocations(this.page, this.paginationSize);
          $('#floatalert').addClass('successmsg');
          // this.totalCmpCount--;
          this.successMsg = "Record Deleted Successfully.";
          $('#floatalert').fadeIn();
        }, error => {
          console.error(error);
          $('#floatalert').addClass('errormsg');
          this.successMsg = "Error while deleting record";
          $('#floatalert').fadeIn();
        });
      setTimeout(function () {
        $('#floatalert').removeClass();
        $('#floatalert').fadeOut();
      }, 8000);
    }
  }

  noofRecordstoDisplay(args: any) {
    this.paginationSize = parseInt(args.target.value);
    this.bindLocations(1, this.paginationSize);
  }
  currentPageBind(currPage: any) {
    this.bindLocations(currPage, this.paginationSize);
  }
  nextPage(nextpageNumber: number) {
    this.page = nextpageNumber + 1;
    if (this.page <= this.totalNoofpages) {
      this.pageS = this.pageS + 10;
      this.bindLocations(this.page, this.paginationSize);
    }
  }
  previousPage(prevPageNumber: number) {
    if (prevPageNumber >= 0) {
      this.pageS = this.pageS - 10;
      //this.bindCorporates(this.pageS + 1, this.paginationSize);
    }
  }
}
