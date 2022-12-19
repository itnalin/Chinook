1. Service layer is defined.
	- Separate services have been created for Artist and Playlist.
	- Methods of each module are exposed through an interface that is implemented by the corresponding class.
		Ex: IArtistService is implemented by the ArtistService class.
	
2. All the sessions with the database (dbcontext related) that were executed in server code in Razor, have been moved to the service layer, so that
	service layer can get the responsibility of database sessions.

3. In the PlaylistPage.razor page, OnParametersSet() is invoked in order to update the playlist during switching to another playlist in NavMenu.
    protected override async void OnParametersSet(){}

4. Implementation of FavoriteTrack, UnfavoriteTrack and RemoveTrack private server code methods in PlaylistPage.razor page are completed so that each method achieve the specific
   update operation through the PlaylistService.

5. Implementation of FavoriteTrack, UnfavoriteTrack and RemoveTrack private server code methods in ArtistPage.razor page are completed so that each method achieve the specific
   update operation through the PlaylistService.

6. Implementation of AddTrackToPlaylist private server code methods in ArtistPage.razor page is completed so that it achieves "new playlist creation" and
   "assigning a track to the newly created playlist or an existing playlist" through the PlaylistService.

7. "PlaylistDialog" modal is modified to facilitate adding a track to a new playlist or an existing playlist. Here the functionality is developed
    to allow the user to select only one option "adding a track to an existing playlist" or "adding a track to a new playlist".

8. NavMenu.razor page added HTML code and server code to show the user playlists created.

9. NavMenu.razor page added private server code method to remove a playlist through the PlaylistService.

10. Playlist "My favorite tracks" given as an example in the NavMenu is commented to show the automatic "My favorite tracks" playlist that is programmatically created.

11. server code private methods GetArtists and GetAlbumsForArtist in Index.razor are moved to ArtistService. 

12. Index.razor page adds HTML code and private server code method to implement Artist search functionality. HTML code adds a Search input box where the user
    needs to type the artist name and press enter. Private method GetArtists consumes the GetArtists method in the ArtistService to get list artists according to the
	searched name.
