import { Component, OnInit, ViewChild, AfterViewInit, OnChanges, AfterContentInit, AfterViewChecked } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { AlertifyService } from "src/app/_services/alertify.service";
import { ActivatedRoute } from "@angular/router";
import { User } from "src/app/_models/User";
import {
  NgxGalleryOptions,
  NgxGalleryImage,
  NgxGalleryAnimation
} from "ngx-gallery";
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: "app-member-detail",
  templateUrl: "./member-detail.component.html",
  styleUrls: ["./member-detail.component.css"]
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;
  selectedTab: number = 0;
 
  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    // this.loadUser();
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.route.queryParams.subscribe(params => {
      this.selectedTab = params['tab'];
      // this.staticTabs.tabs[this.selectedTab>0? this.selectedTab : 0].active = true;
     });
    this.galleryOptions = [
      {
        width: "500px",
        height: "500px",
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        imagePercent: 100,
        preview: false
      },
        { "imageArrowsAutoHide": true, "thumbnailsArrowsAutoHide": true },
        { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
        { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 },
        
      // max-width 800
      {
        breakpoint: 800,
        width: "100%",
        height: "600px",
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();
  }

  getImages() {
    const imagesUrls = [];
    for (const photo of this.user.photos) {
        imagesUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    }
    return imagesUrls;
  }

  // members/2
  loadUser() {
    const id = this.route.snapshot.params["id"];
    this.userService.getUser(+id).subscribe(
      (user: User) => {
        this.user = user;
      },
      error => this.alertifyService.error(error)
    );
  }

  selectTab(tabId: number) {
    this.staticTabs.tabs[tabId].active = true;
  }
}
