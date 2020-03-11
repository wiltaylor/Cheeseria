import { Component, OnInit } from '@angular/core';
import { CheeseService, Cheese } from 'src/app/services/cheese.service';

@Component({
  selector: 'app-cheeselist',
  templateUrl: './cheeselist.component.html',
  styleUrls: ['./cheeselist.component.css']
})
export class CheeselistComponent implements OnInit {

  constructor(private cheeseService: CheeseService) { }

  AllCheese: Cheese[];

  ngOnInit(): void {
    this.cheeseService.allpublished().subscribe(data => {
      this.AllCheese = data;

      console.log(data);
    })
  }
}
